defmodule UnsocialVR.Mixfile do
  use Mix.Project

  def project do
    [
      app: :unsocial_vr,
      version: "0.1.0",
      elixir: "~> 1.4",
      build_embedded: Mix.env == :prod,
      start_permanent: Mix.env == :prod,
      deps: deps(),
      aliases: aliases(),
    ]
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
      {:httpotion, "~> 3.0"},
      {:gproc, "~> 0.6.1"},
      {:logger_file_backend, "~> 0.0.10"},
      {:credo, "~> 0.8.3", only: [:dev, :test], runtime: false},
      {:exsync, "~> 0.1.4", only: :dev},
    ]
  end

  defp aliases do
    [
      test: "test --no-start",
    ]
  end
end
