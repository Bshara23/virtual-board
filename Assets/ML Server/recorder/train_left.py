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

# concatenate the two datasets into the final training set
X_train = np.vstack([openx, point, pinch, closed])

# fit a Gaussian Mixture Model with two components
clf = mixture.GaussianMixture(n_components=4, covariance_type='full')
clf.fit(X_train)

filename = 'gesture_recg_left.sav'
pickle.dump(clf, open(filename, 'wb'))

loaded_model = pickle.load(open(filename, 'rb'))


e = loaded_model.predict(openx)
print(e)

e = loaded_model.predict(point)
print(e)

e = loaded_model.predict(pinch)
print(e)

e = loaded_model.predict(closed)
print(e)