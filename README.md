Unsocial VR
===========

Tom Gurion's Advance Project Placement at Inition.
Part of the Media and Arts Technology doctoral programme, Queen Mary University of London.

## What?

An experiment in using virtual reality to support side conversations.
In reality, side conversation often disturb each other, and you can't pretend to participate in one side conversation when you pay attention to another one.
But in VR you can!

Unsocial VR is designed for multiple remote participants that share the same virtual environment using the HTC Vive.
Compared to real-life conversation the environment in Unsocial VR is not the same for all participants.
When participating in a side conversation, the other players will be either ignored, or pretend to listen to you.

## Running the project

You will need:

- Unity 5.5.0f3
- HTC Vive
- elixir 1.4

### Elixir

The social behaviour server is written in elixir, and runs alongside the Unity server.
To run it:

```bash
cd server
mix deps.get
mix run --no-halt
```
