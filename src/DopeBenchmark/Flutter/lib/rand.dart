class Random2 {
  static const int MBIG = 2000000000;
  static const  int MSEED = 161803398;

  int inext = 0;
  int inextp = 0;
  List<int> seedArray = new List<int>.filled(56 ,0, growable:true);

  Random2(int seed) {
    int ii = 0;
    int mj = 0;
    int mk = 0;

    int subtraction = seed.abs();
    mj = MSEED - subtraction;
    seedArray[55] = mj;
    mk = 1;
    for (int i = 1; i < 55; i++) {
      ii = (21 * i) % 55;
      seedArray[ii] = mk;
      mk = mj - mk;
      if (mk < 0) mk += MBIG;
      mj = seedArray[ii];
    }
    for (int k = 1; k < 5; k++) {
      for (int i = 1; i < 56; i++) {
        seedArray[i] -= seedArray[1 + (i + 30) % 55];
        if (seedArray[i] < 0) seedArray[i] += MBIG;
      }
    }
    inext = 0;
    inextp = 21;
  }

  double sample() {
    return (internalSample() * (1.0 / MBIG));
  }

  int internalSample() {
    int retVal = 0;
    int locINext = inext;
    int locINextp = inextp;

    if (++locINext >= 56) locINext = 1;
    if (++locINextp >= 56) locINextp = 1;

    retVal = seedArray[locINext] - seedArray[locINextp];

    if (retVal == MBIG) retVal--;
    if (retVal < 0) retVal += MBIG;

    seedArray[locINext] = retVal;

    inext = locINext;
    inextp = locINextp;

    return retVal;
  }

  double nextDouble() {
    return sample();
  }
}