defmodule UnsocialVR.SceneAnalysis do

  @doc """
  How I see the other players.
  """
  def remote_players(local_id) do
    all_players = UnsocialVR.PlayersStash.data()
    side_conversations = analyze_side_conversations(all_players)
    {me, the_others} = Map.pop(all_players, local_id)
    me = Map.put(me, :id, local_id)
    the_others = Enum.map(the_others, fn {id, value} -> Map.put(value, :id, id) end)
    player_perspective(me, the_others, side_conversations)
  end

  @doc """
  When less than 2 players are talking everyone is in the same conversation.
  When at least 2 players are talking side coversations are formed as:
  - Each talker form a side conversation.
  - All the chains of attention that end with a talker are in the talker
    side conversation.

  Returns a list of sets of IDs. One set per side conversation.
  """
  def analyze_side_conversations(all_players) do
    talkers =
      all_players
      |> Stream.filter(fn {_id, value} -> value["isTalking"] end)
      |> Enum.map(fn {id, _} -> id end)
    silent_attention_map =
      all_players
      |> Stream.filter(fn {id, _value} -> !(id in talkers) end)
      |> Stream.map(fn {id, value} -> {id, value["attentionTo"]} end)
      |> Enum.into(Map.new())

    Enum.map(talkers, &attention_chain(&1, silent_attention_map))
    |> IO.inspect()
  end

  @doc """
  The players that pay attention to the talker, either directly, or through
  other player.

  ids - A single ID (integer) or a set of IDs.
  attention_map - A map of ID -> ID of all of the non talkers attention.
  Returns a set of IDs attending to the ids in the input.
  """
  def attention_chain(id, attention_map) when is_integer(id) do
    attention_chain(MapSet.new([id]), attention_map)
  end
  def attention_chain(ids, attention_map) do
    {first_order_attendies, the_rest} = group_by(attention_map, fn {_from, to} -> to in ids end)
    updated_ids =
      first_order_attendies
      |> Stream.map(fn {from, _to} -> from end)
      |> Enum.into(ids)
    if updated_ids == ids do
      updated_ids
    else
      attention_chain(updated_ids, the_rest)
    end
  end

  @doc """
  How the player see the world, based on his data, the other players,
  and side conversations analysis.
  """
  def player_perspective(_me, the_others, side_conversations)
  when length(side_conversations) < 2 do
    the_others
  end

  def player_perspective(me, the_others, side_conversations) do
    side_conversation = Enum.find(side_conversations, &MapSet.member?(&1, me.id))
    # TODO if not paying attention to any conversation choose a conversation
    # based on physical distance.
    {attendies, the_rest} = group_by(the_others, &MapSet.member?(side_conversation, &1.id))
    the_rest_state = if me["isTalkin"], do: :autopilot, else: :ignored
    the_rest = Enum.map(the_rest, &Map.put(&1, :state, the_rest_state))
    attendies ++ the_rest
  end

  defp group_by(enumerable, function) do
    grouped = Enum.group_by(enumerable, function)
    {Map.get(grouped, true, []), Map.get(grouped, false, [])}
  end

end
