defmodule UnsocialVR.Autopilot do
  @moduledoc """
  Players in UnsocialVR can pretend to listen to you. What you see in this
  case is a player in "autopilot" mode.
  The autopilot mode is a recorded behaviour of the last few seconds of a
  real behaviour of the player.
  """

  use GenServer
  alias UnsocialVR.Autopilot.Buffer
  require Logger

  @recording_period 100  # millis
  @buffer_size 20

  # INTERFACE

  @doc """
  Play my prerecorded autopilot data.
  """
  def play(id) do
    {:ok, pid} = where_or_start_link(id)
    GenServer.call(pid, :play)
  end

  def record(id, player_data) do
    {:ok, pid} = where_or_start_link(id)
    GenServer.cast(pid, {:record, player_data})
  end

  def set_recording(id, is_recording) do
    {:ok, pid} = where_or_start_link(id)
    GenServer.cast(pid, {:set_recording, is_recording})
  end

  # CALLBACKS

  def init(state) do
    schedule_buffer_rotation()
    {:ok, state}
  end

  def handle_call(:play, _from, %{buffer: buffer} = state) do
    transforms = Buffer.get(buffer)
    {:reply, transforms, state}
  end

  def handle_cast({:record, transforms}, %{buffer: buffer, is_recording: true} = state) do
    {:noreply, %{state | buffer: Buffer.set(buffer, transforms)}}
  end
  def handle_cast({:record, _transforms}, %{is_recording: false} = state) do
    # Just do nothing.
    {:noreply, state}
  end

  @recording_strategies %{true: :forwards, false: :backwards_forwards}

  def handle_cast({:set_recording, is_recording}, %{buffer: buffer}) do
    buffer = Buffer.change_strategy(buffer, @recording_strategies[is_recording])
    {:noreply, %{is_recording: is_recording, buffer: buffer}}
  end

  def handle_info(:rotate_buffer, %{buffer: buffer} = state) do
    buffer = Buffer.rotate(buffer)
    schedule_buffer_rotation()
    {:noreply, %{state | buffer: buffer}}
  end

  # INTERNALS

  defp where_or_start_link(id) do
    # The elixir version to get_or_create a registered process :)
    case :gproc.where(gproc_key(id)) do
      :undefined -> start_link(id)
      pid -> {:ok, pid}
    end
  end

  defp start_link(id) do
    state = %{
      buffer: Buffer.new(@buffer_size),
      is_recording: true,
    }
    GenServer.start_link(__MODULE__, state, name: via_tuple(id))
  end

  defp via_tuple(id) do
    {:via, :gproc, gproc_key(id)}
  end

  defp gproc_key(id) do
    {:n, :l, {:player_autopilot, id}}
  end

  defp schedule_buffer_rotation() do
    Process.send_after(self(), :rotate_buffer, @recording_period)
  end
end
