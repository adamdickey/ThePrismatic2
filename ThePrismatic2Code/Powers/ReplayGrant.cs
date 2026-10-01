using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public sealed class ReplayGrant
{
    private readonly Dictionary<CardModel, int> _granted = new();

    public void Sync(IEnumerable<CardModel> cards, Func<CardModel, bool> qualifies, int amount)
    {
        foreach (CardModel card in cards)
        {
            int wanted = qualifies(card) ? amount : 0;
            if (!_granted.TryGetValue(card, out int current))
            {
                if (card.IsClone)
                {
                    _granted[card] = wanted;
                    continue;
                }
                current = 0;
            }
            if (wanted == current)
            {
                continue;
            }
            card.BaseReplayCount += wanted - current;
            if (wanted == 0)
            {
                _granted.Remove(card);
            }
            else
            {
                _granted[card] = wanted;
            }
        }
    }

    public void Clear()
    {
        foreach (KeyValuePair<CardModel, int> entry in _granted)
        {
            entry.Key.BaseReplayCount -= entry.Value;
        }
        _granted.Clear();
    }
}
