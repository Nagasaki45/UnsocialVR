defmodule BufferTest do
  use ExUnit.Case
  doctest UnsocialVR.Autopilot.Buffer

  import UnsocialVR.Autopilot.Buffer

  test "set and get" do
    buffer = new(1)
    assert get(buffer) == nil
    buffer = set(buffer, :hello)
    assert get(buffer) == :hello
  end

  test "size" do
    buffer = new(3)
    assert size(buffer) == 3
  end

  test "rotate" do
    buffer = new(2)
    buffer = set(buffer, :hello)
    buffer = rotate(buffer)
    buffer = set(buffer, :world)
    buffer = rotate(buffer)
    assert get(buffer) == :hello
    buffer = rotate(buffer)
    assert get(buffer) == :world
  end

  test "different strategies" do
    buffer =
      ~w(my name is tom)
      |> Enum.reduce(new(4), fn value, buffer -> add(buffer, value) end)

    assert get(buffer) == "my"
    buffer = rotate(buffer)
    assert get(buffer) == "name"
    buffer = change_strategy(buffer, :backwards_forwards)
    buffer = rotate(buffer)
    assert get(buffer) == "my"
    buffer = rotate(buffer)
    assert get(buffer) == "tom"
    buffer = rotate(buffer)
    assert get(buffer) == "is"
    buffer = rotate(buffer)
    assert get(buffer) == "name"
    buffer = rotate(buffer)
    assert get(buffer) == "is"
    buffer = change_strategy(buffer, :forwards)
    buffer = rotate(buffer)
    assert get(buffer) == "tom"
    buffer = rotate(buffer)
    assert get(buffer) == "my"
    buffer = rotate(buffer)
    assert get(buffer) == "name"
    buffer = rotate(buffer)
    assert get(buffer) == "is"
    buffer = rotate(buffer)
    assert get(buffer) == "tom"
    buffer = change_strategy(buffer, :backwards_forwards)
    buffer = rotate(buffer)
    assert get(buffer) == "is"
    buffer = change_strategy(buffer, :forwards)
    buffer = rotate(buffer)
    assert get(buffer) == "name"
    buffer = rotate(buffer)
    assert get(buffer) == "my"
  end

end
