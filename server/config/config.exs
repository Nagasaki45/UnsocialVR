# This file is responsible for configuring your application
# and its dependencies with the aid of the Mix.Config module.
use Mix.Config

# This configuration is loaded before any dependency and is restricted
# to this project. If another project depends on this project, this
# file won't be loaded nor affect the parent project. For this reason,
# if you want to provide default values for your application for
# 3rd-party users, it should be done in your "mix.exs" file.

# You can configure for your application as:
#
#     config :unsocial_vr, key: :value
#
# And access this configuration in your application as:
#
#     Application.get_env(:unsocial_vr, :key)
#
# Or configure a 3rd-party app:
#
#     config :logger, level: :info
#

# UnsocialVR

config :unsocial_vr,
  autopilot_recording_period: 100,  # millis
  autopilot_buffer_size: 20,
  backchannel_server: "http://127.0.0.1:5001",
  gcff_server: "http://127.0.0.1:5000/continuous",
  f_formation_analysis_period: :timer.seconds(1),
  token_score: 5

# 3rd-party

config :logger,
  backends: [
    :console,
    {LoggerFileBackend, :experiment},
  ]

config :logger, :experiment,
  path: "experiment.log",
  level: :info

# It is also possible to import configuration files, relative to this
# directory. For example, you can emulate configuration per environment
# by uncommenting the line below and defining dev.exs, test.exs and such.
# Configuration from the imported file will override the ones defined
# here (which is why it is important to import them last).
#
#     import_config "#{Mix.env}.exs"
