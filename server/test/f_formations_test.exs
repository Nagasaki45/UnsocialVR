defmodule FFormationsTest do
  use ExUnit.Case
  doctest UnsocialVR.FFormations

  import UnsocialVR.FFormations

  test "group by key" do
    items = ~w(moshe prawn jacob)
    keys = ~w(person animal person)a
    expected = [~w(moshe jacob), ~w(prawn)]
    assert MapSet.new(group_by_key(items, keys)) == MapSet.new(expected)
  end
end
