defmodule UnsocialVR.Mixfile do
  use Mix.Project

  def project do
    [app: :unsocial_vr,
     version: "0.1.0",
     elixir: "~> 1.4",
     build_embedded: Mix.env == :prod,
     start_permanent: Mix.env == :prod,
     deps: deps()]
  end

  def application do
    [
      extra_applications: [:logger],
      mod: {UnsocialVR.Application, []},
    ]
  end

  defp deps do
    [
      {:cowboy, "~> 1.1"},
      {:plug, "~> 1.3"},
      {:poison, "~> 3.1"},
      {:con_cache, "~> 0.12.0"},
    ]
  end
end
