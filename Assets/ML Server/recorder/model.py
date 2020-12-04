import numpy as np
import matplotlib.pyplot as plt
from matplotlib.colors import LogNorm
from sklearn import mixture
import pickle
import util.math as m
import math as mc

def preprocess(landmarks):
    landmarks = np.asarray(landmarks)
    c = m.centeroid(landmarks)
    c = np.asarray(c)
    landmarks -= c

    d = abs(landmarks[0] - landmarks[8])
    d = mc.sqrt(d[0] ** 2 + d[1] ** 2)

    res = 1
    scale = 1 / d

    landmarks *= scale

    return np.reshape(landmarks, 42)

filename = '../recorder/gesture_recg.sav'
model_right = pickle.load(open(filename, 'rb'))
filename = '../recorder/gesture_recg_left.sav'
model_left = pickle.load(open(filename, 'rb'))

mapp_right = {
    2: "open",
    3: "point",
    1: "pinch",
    0: "closed",
}

mapp_left = {
    2: "pinch",
    3: "point",
    1: "closed",
    0: "open",
}



def predict(arr, isLeft):
    if isLeft:
        hand = preprocess(arr)
        e = model_left.predict([hand])[0]
        print("left", mapp_left[e])
        return mapp_left[e]
    else:
        hand = preprocess(arr)
        e = model_right.predict([hand])[0]
        print("right", mapp_right[e])
        return mapp_right[e]

