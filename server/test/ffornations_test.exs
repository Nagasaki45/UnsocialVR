defmodule FFormationsTest do
  use ExUnit.Case
  doctest UnsocialVR.FFormations

  import UnsocialVR.FFormations

  test "chunk by key" do
    items = ~w(moshe jacob prawn)
    keys = ~w(person person animal)a
    expected = [~w(moshe jacob), ~w(prawn)]
    assert chunk_by_key(items, keys) == expected
  end
end
