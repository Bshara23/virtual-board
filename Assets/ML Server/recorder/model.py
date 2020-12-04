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
loaded_model = pickle.load(open(filename, 'rb'))

mapp = {
    2: "open",
    3: "point",
    1: "pinch",
    0: "closed",
}


def predict(arr):
    hand = preprocess(arr)
    e = loaded_model.predict([hand])[0]
    return mapp[e]

