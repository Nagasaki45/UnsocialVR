defmodule UnsocialVR.Application do
  use Application

  def start(_type, _args) do
    import Supervisor.Spec, warn: false

    children =
      [
        worker(UnsocialVR.PlayersStash, []),
        Plug.Adapters.Cowboy.child_spec(:http, UnsocialVR.HTTP, [], port: 8080),
      ]

    opts = [strategy: :one_for_one, name: UnsocialVR.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
