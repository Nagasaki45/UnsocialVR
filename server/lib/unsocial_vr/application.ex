defmodule UnsocialVR.Application do
  use Application

  def start(_type, _args) do
    import Supervisor.Spec, warn: false

    con_cache_opts = [
      [ttl_check: 1000, ttl: 1000],
      [name: :cache]
    ]

    children =
      [
        supervisor(ConCache, con_cache_opts),
        Plug.Adapters.Cowboy.child_spec(:http, UnsocialVR.HTTP, [], port: 8080),
        worker(UnsocialVR.Autopilot, [])
      ]

    opts = [strategy: :one_for_one, name: UnsocialVR.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
