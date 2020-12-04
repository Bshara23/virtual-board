

# 1. run cam
# 2. identify if gmm is available
# 3. record on space bar click
# 4. print memory size
# 5. exit on escape, then save

import cv2
import mediapipe as mp

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

def handler(hand):
  landmarks = []
  for hand_landmarks in hand.multi_hand_landmarks:
    landmarks.append([[p.x, p.y] for p in hand_landmarks.landmark])

  landmarks = np.asarray(landmarks)
  landmarks *= (1280, 720)

  c = m.centeroid(landmarks)
  c = np.asarray(c)
  # c *= (1280, 720)
  landmarks = landmarks - c
  print(c)
  #recorder_landmarks.append(landmarks)
  #print(len(recorder_landmarks))

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
      handler(results)

  cv2.imshow('MediaPipe Hands', image)

  key = cv2.waitKey(1)

  if key & 0xFF == 27:
    break

  if key == 32:
    isRecording = not isRecording

hands.close()
cap.release()


