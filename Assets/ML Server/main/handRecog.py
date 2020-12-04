import cv2
import mediapipe as mp
import main.handler as handler
mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands
mp_ha2nds = mp.solutions.hands.thresholding_calculator_pb2

DrawingSpec = mp.solutions.drawing_utils.DrawingSpec

# For webcam input:

hands = mp_hands.Hands(
    min_detection_confidence=0.7, min_tracking_confidence=0.5)
cap = cv2.VideoCapture(1)
while cap.isOpened():
  success, image = cap.read()
  if not success:
    break

  # Flip the image horizontally for a later selfie-view display, and convert
  # the BGR image to RGB.
  image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
  # To improve performance, optionally mark the image as not writeable to
  # pass by reference.
  image.flags.writeable = False
  results = hands.process(image)


  # Draw the hand annotations on the image.
  image.flags.writeable = True

  image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
  if results.multi_hand_landmarks:
    handler.hand_handler(results)
    image = handler.hand_drawer(image)
    for hand_landmarks in results.multi_hand_landmarks:
      mp_drawing.draw_landmarks(
          image, hand_landmarks, mp_hands.HAND_CONNECTIONS,
            landmark_drawing_spec = DrawingSpec(color=(192, 255, 44), circle_radius=2),
            connection_drawing_spec = DrawingSpec(color=(219, 179, 17)))


    for hand in results.multi_handedness:
      pass
      #print(hand.classification[0].label, hand.classification[0].index)
      # print([(x.x ,x.y, x.z) for x in hand_landmarks.landmark][0])

  cv2.imshow('Hands', image)
  if cv2.waitKey(1) & 0xFF == 27:
    break
hands.close()
cap.release()