![logo](graphics/logo_low_res.png)

Advanced Placement Project, Media and Arts Technology doctoral programme, Queen Mary University of London. Supervised by Prof. Patrick Healey and hosted at Inition.

## What?

In Infinite Jest, David Foster Wallace argues that "Good old traditional audio-only phone conversations allowed you to presume that the person on the other end was paying complete attention to you while also permitting you not to have to pay anything even close to complete attention to her." He continues and claims that we are addicted to this illusion, and that's why video conferencing always feel so awkward - we need to pretend to listen all the time. And if we think about it, even in face to face conversation we must always adhere to these social rules, and signal our complete attention when someone is talking to us.

In this project I experiment with VR technologies to see if this illusion of faking active listening is transferable to other mediums, and if so, how. In Unsocial VR participants share the same virtual environment, using the HTC Vive headset and controllers. They can converse freely and move around, and if you want to start faking listening to the conversation and just wander around, or even talk with other participants while faking, you absolutely can! The interface is very minimal, just hit a button on the controller to start faking active listening behaviours towards your current conversation, and release it when you want to stop faking. You will even get an on screen notification when someone is speaking directly to you, so you can return to the conversation elegantly.

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
