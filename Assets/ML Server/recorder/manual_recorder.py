

# 1. run cam
# 2. identify if gmm is available
# 3. record on space bar click
# 4. print memory size
# 5. exit on escape, then save

import cv2
import mediapipe as mp
import main.handler as handler
import math as mc
import numpy as np
import util.math as m
mp_drawing = mp.solutions.drawing_utils
DrawingSpec = mp.solutions.drawing_utils.DrawingSpec


mp_hands = mp.solutions.hands


# For webcam input:
hands = mp_hands.Hands(
    min_detection_confidence=0.7, min_tracking_confidence=0.5)
cap = cv2.VideoCapture(0)

recorder_landmarks = []

def record_handler(results):
  global isRecording
  landmarks = handler.hand_handler(results)
  landmarks = np.asarray(landmarks[0])
  c = m.centeroid(landmarks)
  c = np.asarray(c)
  landmarks -= c

  d = abs(landmarks[0] - landmarks[8])
  d = mc.sqrt(d[0]**2 + d[1]**2)

  res = 1
  scale = 1/d

  landmarks *= scale

  recorder_landmarks.append(landmarks)

  if len(recorder_landmarks) == 300:
    save_landmarks("point")
    isRecording = not isRecording
  print(len(recorder_landmarks))


def save_landmarks(name):
  global recorder_landmarks
  recorder_landmarks = np.asarray(recorder_landmarks)
  np.save(f'{name}.npy', recorder_landmarks)  # .npy extension is added if not given
  print(name, recorder_landmarks.shape[0])
  recorder_landmarks = []

isRecording = False
while cap.isOpened():
  success, image = cap.read()
  if not success:
    break

  image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
  image.flags.writeable = False
  results = hands.process(image)
  image.flags.writeable = True

  image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
  if results.multi_hand_landmarks:
    for hand_landmarks in results.multi_hand_landmarks:

      mp_drawing.draw_landmarks(
          image, hand_landmarks, mp_hands.HAND_CONNECTIONS,
        landmark_drawing_spec=DrawingSpec(color=(192, 255, 44), circle_radius=2),
        connection_drawing_spec=DrawingSpec(color=(219, 179, 17)))

    if isRecording:
      record_handler(results)

  cv2.imshow('Hands', image)

  key = cv2.waitKey(1)

  if key & 0xFF == 27:
    break

  if key == 32:
    isRecording = not isRecording


hands.close()
cap.release()


