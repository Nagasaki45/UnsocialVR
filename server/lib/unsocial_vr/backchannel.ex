defmodule UnsocialVR.Backchannel do
  @moduledoc """
  Backchannel behaviours are social cues for listening, like head nods.
  This module periodically fetchs autopilots data from the cache, find
  the appropriate speakers, and predict head nods using existing server
  written in python for the job.
  """

  use GenServer

  # INTERFACE

  def start_link() do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
  end

  def add_prediction_job(autopilot, speaker) do
    args = {:add_prediction_job,
            autopilot.id,
            !speaker["isTalking"],
            speaker["attention"] == autopilot.id}
    GenServer.cast(__MODULE__, args)
  end

  # CALLBACKS

  def init(prediction_jobs) do
    schedule_prediction()
    {:ok, prediction_jobs}
  end

  def handle_info(:predict, prediction_jobs) do
    schedule_prediction()

    prediction_jobs
    |> predict()
    |> cache_results()

    {:noreply, %{}}
  end

  def handle_cast({:add_prediction_job, id, silence, gaze}, prediction_jobs) do
    {:noreply, Map.put(prediction_jobs, id, [silence, gaze])}
  end

  # INTERNALS

  def schedule_prediction() do
    Process.send(__MODULE__, :predict, [])
  end

  @doc """
  Call the server to generate prediction.
  """
  def predict(data) do
    data = %{listeners: data, type: "dekok"}
    body = Poison.encode!(data)
    headers = ["Content-Type": "application/json"]
    url = Application.get_env(:unsocial_vr, :backchannel_server)
    resp = HTTPotion.post(url, body: body, headers: headers)
    %{status_code: 200, body: body} = resp
    Poison.decode!(body)
  end

  def cache_results(predictions) do
    Enum.each(predictions, fn {id, prediction} ->
      UnsocialVR.Cache.put_backchannel(String.to_integer(id), prediction)
    end)
  end

end
