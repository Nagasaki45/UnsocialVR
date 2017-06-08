defmodule AutopilotTest do
  use ExUnit.Case
  doctest UnsocialVR.Autopilot

  import UnsocialVR.Autopilot

  test "position shift" do
    data = %{"somePosition" => %{"x" => 1, "y" => 1, "z" => 1}}
    result = shift_positions(data, {1, 2})
    assert result["somePosition"]["x"] == 2
    assert result["somePosition"]["y"] == 1
    assert result["somePosition"]["z"] == 3
  end

  test "finilize recording shift headPosition to origin" do
    data = [
      {0, %{"headPosition" => %{"x" => 1, "y" => 1, "z" => 1}}},
      {1, %{"headPosition" => %{"x" => 2, "y" => 2, "z" => 2}}},
    ]
    expected = [
      {0, %{"headPosition" => %{"x" => -0.5, "y" => 1, "z" => -0.5}}},
      {1, %{"headPosition" => %{"x" => 0.5, "y" => 2, "z" => 0.5}}},
    ]
    assert finilize_recording(data) == expected
  end

end
