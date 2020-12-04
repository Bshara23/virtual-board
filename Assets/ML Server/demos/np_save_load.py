
import numpy as np

arr = np.asarray([[1, 5], [2, 3]])
np.save('test3.npy', arr)    # .npy extension is added if not given
d = np.load('test3.npy')

print(d)
