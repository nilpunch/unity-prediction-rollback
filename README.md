# unity-prediction-rollback - Work in progress
Deterministic prediction-rollback netcode library for fast-paced games.

Does not reference Unity Engine, so it could be used in a regular C# project.

## Goals

1. Responsive controls with no delay
2. Unlimited session length with late-joins
3. Easy client-side coding with Unity (no ECS)
4. Clean and simple OOP architecture
5. Cross-platform determinism

## Overview

It is a **library**, not a framework. So, the user is in control of everything.

Implemented features:

1. Prediction-rollback with zero GC allocations
2. Live command timeline - make changes in the past and see immediate changes in the future
3. Distributed simulation - killer feature, allowing for many great things
4. Algorithmic command prediction - replication, fading out, anything is possible!
5. Snapshots memory recycling, if needed
6. Replays with command history

TODO:

1. Commands serialization
2. Late-joins via distributed state serialization
3. Actual networking

## Recent progress

Here is some simple shooter with live simulation replay.

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/574b9dac-3b13-4032-9524-b2339ea46fd8
