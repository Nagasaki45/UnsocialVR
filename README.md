![logo](graphics/logo_low_res.png)

Advance Project Placement, Media and Arts Technology doctoral programme, Queen Mary University of London.

## What?

An experiment in using virtual reality to support side conversations.
In reality, side conversation often disturb each other, and you can't pretend to participate in one side conversation when you pay attention to another one.
But in VR you can!

Unsocial VR is designed for multiple remote participants that share the same virtual environment using the HTC Vive.
Compared to real-life conversation the environment in Unsocial VR is not the same for all participants.
When participating in a side conversation, the other players will be either ignored, or pretend to listen to you.

## Running the project

You will need:

- Unity 5.6.0f3
  - Dissonance Voice Chat 1.1.0. You will need to buy it from the assets store.
- HTC Vive
- elixir 1.4
- Whatever required to run the GCFF (Graph-Cut for F-formations) server. See `GCFF/README.md`.

### Servers

Run the GCFF server with appropriate `stride` and `mdl` values. It seems that 1 and 10 work fine:

```bash
cd GCFF
python server.py 1 10
```

The social behaviour server is written in elixir. It communicate with the GCFF server to analyze the social scene, and runs alongside the Unity server.
To run it:

```bash
cd server
mix deps.get
mix run --no-halt
```

## Recording new autopilot behaviour

Players in UnsocialVR can pretend to listen to you. What you see in this case is a player in "autopilot" mode. It's a recorded behaviour.

To record new autopilot behaviour run the unity project as a server + client, and then:

```bash
cd server
iex -S mix
iex> UnsocialVR.Autopilot.start_recording(1)  # 1 is for the player_id you want to record
iex> # Move freely with the Vive and its controllers
iex> UnsocialVR.Autopilot.stop_recording()
```
