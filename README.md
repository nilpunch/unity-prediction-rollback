# unity-prediction-rollback - Work in progress

Deterministic prediction-rollback netcode library for fast-paced games.

UNSTABLE, DO NOT USE IN PRODUCTION. Stable version will come soon, and will be based on https://github.com/nilpunch/massive

Repository was reseted 28.01.2024. Previous version performs really well on memory consumption, but was awkward to work with and has many issues with CPU performance, and its structure is basically impossible to serialize.

## Goals

1. Responsive controls with no delay
2. Unlimited session length with late-joins
3. Clean and simple architecture
4. Cross-platform determinism

## Overview (before 28.01.2024)

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

## Progress (before 28.01.2024)

Here is some simple shooter with prediction-rollback features.

Input prediction sample. When there is no input from the player, the movement is fading out, and the shooting repeates in the last direction.

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/d2231a6d-28a7-4ae7-840f-ae9a2982e8cc

Live simulation replay sample.

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/574b9dac-3b13-4032-9524-b2339ea46fd8
