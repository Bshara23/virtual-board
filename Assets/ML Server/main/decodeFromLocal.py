import cv2
import socket


# Should decode frames from camera, process them, then send the calculations
# to the unity client

print('server starts...')

ip_address = '127.0.0.1'
port_number = 7000
BUFFER_SIZE = 4096  # TODO: find the optimal buffer size

# using tcp instead of udp
ss = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
ss.bind((ip_address, port_number))
ss.listen(1)
cs, addr = ss.accept()


cap = cv2.VideoCapture(1)  # listen to a virtual camera since the main one will be used by unity
while cap.isOpened():
  success, image = cap.read()
  if not success:
    break

  # test sending
  try:
    cs.send(bytes("res", "utf-8"))
  except:
    cs.send(bytes("no res", "utf-8"))

  cv2.imshow('Camera View', image)
  if cv2.waitKey(1) & 0xFF == 27:
    break


cap.release()
cs.close()
