defmodule UnsocialVR.Autopilot.Buffer do
  @moduledoc """
  A fixed size buffer that support set, get and rotate operations. The cursr
  can have one of two strategies for rotating: `:forwards` (default) or
  `:backwords_forwards`.
  """
  defstruct data: %{}, cursor: 0, size: 0, step: 1,
            strategy: :forwards, zero_point: 0

  @doc """
  Create a new buffer with fixed size.
  """
  def new(size) do
    data = for x <- 1..(size - 1), do: {x, nil}, into: Map.new()
    %__MODULE__{data: data, size: size}
  end

  @doc """
  Write a value to te current write position.
  """
  def set(%__MODULE__{data: data, cursor: cursor} = buffer, value) do
    %{buffer | data: Map.put(data, cursor, value)}
  end

  @doc """
  Read a value from the current read position.
  """
  def get(buffer) do
    buffer.data[buffer.cursor]
  end

  @doc """
  Rotate the buffer one step according to its strategy.
  """
  def rotate(%__MODULE__{strategy: :forwards} = buffer) do
    cursor = mod(buffer.cursor + buffer.step, buffer.size)
    %{buffer | cursor: cursor}
  end
  def rotate(%__MODULE__{strategy: :backwards_forwards} = buffer) do
    cursor = mod(buffer.cursor + buffer.step, buffer.size)
    step_multiplier = if cursor == buffer.zero_point, do: -1, else: 1
    step = buffer.step * step_multiplier
    %{buffer | cursor: cursor, step: step}
  end

  @doc """
  Same as `buffer |> set(value) |> rotate()`.
  """
  def add(buffer, value) do
    buffer |> set(value) |> rotate()
  end

  def size(%__MODULE__{size: size}) do
    size
  end

  def change_strategy(buffer, :backwards_forwards) do
    %{buffer | strategy: :backwards_forwards,
               step: buffer.step * -1, zero_point: buffer.cursor}
  end
  def change_strategy(buffer, :forwards) do
    %{buffer | strategy: :forwards}
  end

  def mod(x, y) when x > 0, do: rem(x, y)
  def mod(x, y) when x < 0, do: rem(x + y, y)
  def mod(0, _), do: 0
end
