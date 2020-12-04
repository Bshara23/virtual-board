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


openx = lr('l_open')
point = lr('l_point')
pinch = lr('l_pinch')
closed = lr('l_closed')



filename = 'gesture_recg_left.sav'

loaded_model = pickle.load(open(filename, 'rb'))




e = loaded_model.predict([openx[0]])[0]

mapp = {
    2: "pinch",
    3: "point",
    1: "closed",
    0: "open",
}


print(mapp[e])
