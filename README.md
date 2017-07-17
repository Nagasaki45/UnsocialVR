![logo](graphics/logo_low_res.png)

Advance Project Placement, Media and Arts Technology doctoral programme, Queen Mary University of London.

## What?

An experiment in using virtual reality to support side conversations.
In reality, side conversation often disturb each other, and you can't pretend to participate in one side conversation when you pay attention to another one.
But in VR you can!

Unsocial VR is designed for multiple remote participants that share the same virtual environment using the HTC Vive.
Compared to real-life conversation the environment in Unsocial VR is not the same for all participants.
When participating in a side conversation, the other players will be either ignored, or pretend to listen to you.

There are some extra details [on my site](http://www.tomgurion.me/unsocial-vr.html) and a short presentation [here](https://www.youtube.com/watch?v=K39_wlQ60-Y).

## Running the project

You will need:

- Unity 5.6.0f3
  - Dissonance Voice Chat 1.1.0. You will need to buy it from the assets store.
- HTC Vive
- elixir 1.4
- Whatever required to run the GCFF (Graph-Cut for F-formations) server. See `GCFF/README.md`.
- Whatever required to run the backchannel server (predicting head nods). See `backchannel/README.md`.

### Servers

Run the GCFF server with appropriate values for `stride`, `mdl`, and `max_change_per_second`. It seems that these values work fine:

```bash
cd GCFF
python server.py 1 10 3
```

A separate python server is responsible for predicting listener backchannels (head nods), according to speaker behaviours (is the speaker talking, etc.).
Run it with:

```bash
cd backchannel
python server.py --port 5001  # Make sure it's on port 5001!
```

The social behaviour server is written in elixir. It communicate with the two other servers to pass positioning data between the clients, analyze the social scene, and generate appropriate backchannels when necessary.
It runs alongside the Unity server.
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
