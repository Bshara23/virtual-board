import cv2
import mediapipe as mp
import main.handler as handler
import socket

mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands
mp_ha2nds = mp.solutions.hands.thresholding_calculator_pb2

DrawingSpec = mp.solutions.drawing_utils.DrawingSpec

print('server starts...')

ip_address = '127.0.0.1'
port_number = 7000
BUFFER_SIZE = 4096

ss = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
ss.bind((ip_address, port_number))
ss.listen(1)
cs, addr = ss.accept()

hands = mp_hands.Hands(
    min_detection_confidence=0.7, min_tracking_confidence=0.5)
cap = cv2.VideoCapture(1)
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
    landmarksx = handler.hand_handler(results)
    image = handler.hand_drawer(image)
    for hand_landmarks in results.multi_hand_landmarks:
      mp_drawing.draw_landmarks(
          image, hand_landmarks, mp_hands.HAND_CONNECTIONS,
            landmark_drawing_spec = DrawingSpec(color=(192, 255, 44), circle_radius=2),
            connection_drawing_spec = DrawingSpec(color=(219, 179, 17)))

    msg = str(landmarksx)
    try:
        cs.send(bytes(msg, "utf-8"))
    except:
        cs.send(bytes("no res", "utf-8"))
  else:
      cs.send(bytes("no res", "utf-8"))

  cv2.imshow('MediaPipe Hands', image)
  if cv2.waitKey(1) & 0xFF == 27:
    break
hands.close()
cap.release()