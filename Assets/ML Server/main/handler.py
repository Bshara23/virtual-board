
import numpy as np
import cv2
import main.drawer as draw
from collections import deque
from sklearn import mixture
import json

import pickle
left_x = deque(maxlen=2)
left_x.append([0 ,0])
right_x = deque(maxlen=2)

firstIsLeft = False
onlyLeft = False
onlyRight = False
both = False
landmarks = []

w = 640
h = 480

isLeft = False
leftIndex = 0
isRight = False
rightIndex = 0

import recorder.model as gesture_model


def hand_handler(hand):
    global landmarks, isLeft, leftIndex, isRight, rightIndex

    landmarks = []
    for hand_landmarks in hand.multi_hand_landmarks:
        landmarks.append([[p.x, p.y] for p in hand_landmarks.landmark])

    x = hand.multi_handedness[0].classification[0].index
    y = 1
    try:
        y = hand.multi_handedness[1].classification[0].index
        z = 1
    except:
        z = 0

    # both or left only
    if x == 0 and y == 1:
        isLeft = True
        isRight = False
        leftIndex = 0
        if z == 1:
            isRight = True
            rightIndex = 1

    # both
    if x == 1 and y == 0:
        isLeft = True
        isRight = True
        leftIndex = 1
        rightIndex = 0

    # right hand only
    if x == 1 and y == 1:
        isRight = True
        isLeft = False
        rightIndex = 0

    js = {}
    js['left'] = []
    js['right'] = []
    js['leftGesture'] = ""
    js['leftGesture'] = ""
    if isLeft:
        js['left'] = [{"x": pos[0], "y": pos[1]} for pos in landmarks[leftIndex]]
        js['leftGesture'] = gesture_model.predict(landmarks[leftIndex], isLeft=True)
    if isRight:
        js['right'] = [{"x": pos[0], "y": pos[1]} for pos in landmarks[rightIndex]]
        js['rightGesture'] = gesture_model.predict(landmarks[rightIndex], isLeft=False)
    js_str = json.dumps(js)
    # print(js_str)

    return js_str


def get_point(left, idx):
    global landmarks, isRight, rightIndex, isLeft, leftIndex
    if left:
        return (int(np.rint(landmarks[leftIndex][idx][0] * w)), int(np.rint(landmarks[leftIndex][idx][1] * h)))
    else:
        return (int(np.rint(landmarks[rightIndex][idx][0] * w)), int(np.rint(landmarks[rightIndex][idx][1] * h)))

def hand_drawer(image):
    global isRight, isLeft
    #image = draw_all_points(image)

    if isLeft:
        image = leftHandHandler(image)

    if isRight:
        image = rightHandHandler(image)

    return image

def leftHandHandler(image):
    global leftIndex, left_x
    image = draw_point(image, 8, leftIndex, 'L')

    left_x.append(get_point(True, 8))

    dx = left_x[0][0] - left_x[1][0]
    leftDisplacementHandler(dx)
    #print("dx left:", dx, len(left_x))


    return image

def rightHandHandler(image):
    global rightIndex
    image = draw_point(image, 8, rightIndex, 'R')

    return image


def leftDisplacementHandler(dx):

    s = "Left" if dx > 0 else "Right"
    if abs(dx) > 200:
        print(s, abs(dx))

def draw_all_points(image):

    for i in range(len(landmarks)):
        center = (int(np.rint(landmarks[i][0] * w)), int(np.rint(landmarks[i][1] * h)))
        image = draw.my_filled_circle(image, center, str(i))

    return image


def draw_point(image, idx, handIndex, txt):
    try:
        center = (int(np.rint(landmarks[handIndex][idx][0] * w)), int(np.rint(landmarks[handIndex][idx][1] * h)))
        image = draw.my_filled_circle(image, center, txt)
    except:
        print("err")
        return image
    return image
