
import numpy as np
from timeit import timeit

def centeroid(arr):
    length = arr.shape[0]
    sum_x = np.sum(arr[:, 0])
    sum_y = np.sum(arr[:, 1])
    return sum_x/length, sum_y/length

if __name__ == '__main__':
    cxy = [(1, 1), (2, 3)]
    cxy = np.asarray(cxy)
    x = timeit(lambda: centeroid(cxy), number=1000)

    print(x)
    # print(cxy[:, 1])
