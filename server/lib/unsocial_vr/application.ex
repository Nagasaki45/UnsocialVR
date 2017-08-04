defmodule UnsocialVR.Application do
  @moduledoc """
  The social behaviour server OTP application.
  """

  use Application


  def start(_type, _args) do
    import Supervisor.Spec, warn: false

    concache_opts = [ttl_check: 2000, ttl: 2000, touch_on_read: true]

    children =
      [
        supervisor(ConCache, [concache_opts, [name: :cache]]),
        Plug.Adapters.Cowboy.child_spec(:http, UnsocialVR.HTTP, [], port: 8080),
        worker(UnsocialVR.Backchannel, []),
        worker(UnsocialVR.Scores, []),
      ]

    opts = [strategy: :one_for_one, name: UnsocialVR.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
