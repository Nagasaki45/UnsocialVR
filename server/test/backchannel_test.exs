def BackchannelTest do
  use ExUnit.Case
  doctest UnsocialVR.Backchannel

  import UnsocialVR.Backchannel

  test "prepare data for prediction" do
    # Just an example of some fields from player_data.
    data = [
      %{:id => 1, "isTalking" => true, "attention" => 3}
      %{:id => 2, "isTalking" => false, "attention" => 4}
    ]
    expected = %{
      1 => [false, 0]
    }
  end
end
