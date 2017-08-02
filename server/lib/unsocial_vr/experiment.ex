defmodule UnsocialVR.Experiment do
  @moduledoc """
  Logging all experiment relaetd events to file.
  """

  @log_path "experiment.log"

  def log(player_id, content) do
    participant_id = UnsocialVR.Participants.get(player_id)
    timestamp = System.monotonic_time(:millisecond)
    msg = format(timestamp, participant_id, content)
    write_to_log(msg)
  end

  defp write_to_log(msg) do
    {:ok, file} = File.open(@log_path, [:append])
    IO.write(file, msg)
    File.close(file)
  end

  defp format(timestamp, participant_id, content) do
    "#{timestamp},#{participant_id},#{content}\n"
  end
end
