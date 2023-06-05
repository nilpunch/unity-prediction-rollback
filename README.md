# unity-prediction-rollback - Work in progress
Deterministic prediction-rollback netcode library for fast-paced games.
Does not reference Unity Engine, so it could be used in a regular C# project.

## Goals

1. Responsive controls with no delay
2. Unlimited session length
3. Clean and simple OOP architecture
4. Traditional client-side visualisation with Unity (no ECS)
5. Cross-platform determinism

## Implemented features

1. Prediction-rollback with zero GC allocations
2. Live command timeline - make changes in the past and see immediate results in realtime
3. Distributed simulation state - killer feature, allowing for many great things

## TODO

1. Late-joins via distributed state serialization
2. History rebase to limit memory consumption in the long run
3. Actual networking

## Recent progress

https://github.com/nilpunch/unity-prediction-rollback/assets/69798762/574b9dac-3b13-4032-9524-b2339ea46fd8
