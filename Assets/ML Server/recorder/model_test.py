import numpy as np
import matplotlib.pyplot as plt
from matplotlib.colors import LogNorm
from sklearn import mixture


import pickle

def lr(name):
    d = np.load(f'{name}.npy')
    res = []
    for x in d:
        res.append(np.reshape(d[0], 42))

    return res


openx = lr('open')
point = lr('point')
pinch = lr('pinch')
closed = lr('closed')



filename = 'gesture_recg.sav'

loaded_model = pickle.load(open(filename, 'rb'))




e = loaded_model.predict([closed[0]])[0]

mapp = {
    2: "open",
    3: "point",
    1: "pinch",
    0: "open",
}


print(mapp[e])
