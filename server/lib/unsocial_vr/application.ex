defmodule UnsocialVR.Application do
  use Application

  def start(_type, _args) do
    import Supervisor.Spec, warn: false

    players_con_cache_opts = [
      [ttl_check: 1000, ttl: 1000],
      [name: :cache]
    ]

    f_formations_con_cache_opts = [[], [name: :f_formations]]

    children =
      [
        supervisor(ConCache, players_con_cache_opts, id: :cache),
        supervisor(ConCache, f_formations_con_cache_opts, id: :f_formations),
        Plug.Adapters.Cowboy.child_spec(:http, UnsocialVR.HTTP, [], port: 8080),
        worker(UnsocialVR.Autopilot, []),
        worker(UnsocialVR.FFormations, []),
      ]

    opts = [strategy: :one_for_one, name: UnsocialVR.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
