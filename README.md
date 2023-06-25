# unity-prediction-rollback - Work in progress
Deterministic prediction-rollback netcode library for fast-paced games.

Library does not reference Unity Engine, so it could be used in a regular C# project.

P.S. currently, only samples references Unity Engine.

## Goals

1. Responsive controls with no delay
2. Unlimited session length with late-joins
3. Easy client-side coding with Unity (no ECS)
4. Clean and simple OOP architecture
5. Cross-platform determinism

## Overview

This is a **library**, not a framework. Thus, the user controls everything that happens.

Implemented features:

1. Prediction-rollback with zero GC allocations
2. Live command timeline - make changes in the past and see immediate effect upon the future
3. Distributed simulation - killer feature, allowing for many great things
4. Algorithmic command prediction
   * Input replication
   * Input decay (fading out)
5. Snapshots memory recycling on demand
6. Replays with command history

TODO:

1. Commands serialization
2. State serialization for late-joins
3. Actual networking

## Recent progress

Here is some simple shooter with prediction-rollback features.

Input prediction sample. When there is no input from the player, the movement is fading out, and the shooting repeates in the last direction.

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/d2231a6d-28a7-4ae7-840f-ae9a2982e8cc

Live simulation replay sample.

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/574b9dac-3b13-4032-9524-b2339ea46fd8
