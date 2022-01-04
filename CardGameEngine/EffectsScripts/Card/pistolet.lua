max_level = 2
image_id = 569


name = "Pistolet"
pa_cost = 2

targets = {
    CreateTarget("met une carte dans sa deffause (1 -> carte avec cout d'action plus bas 2 -> carte avec cout d'action plus haut", TargetTypes.Card, true, cardFilter),
}

function cardFilter()
    local OtherHand = EffectOwner.OtherPlayer.Hand
    if (OtherHand.Count==0)then
        return nil
    elseif(current_level == 1)then
        local minCard = OtherHand[1].Cost.Value
        for card in OtherHand do
            if (card.Cost.Value < minCard)then
                minCard=card.Cost.Value
            end
        end
        return minCard
    else
        local maxCard = OtherHand[1].Cost.Value
        for card in OtherHand do
            if (card.Cost.Value > maxCard)then
                maxCard =card.Cost.Value
            end
        end
        return maxCard
    end
end


function precondition()
    return TargetsExists({ 1 })
end

function description()
    return "met une carte dans sa deffause (1 -> carte avec cout d'action plus bas 2 -> carte avec cout d'action plus haut"
end

function do_effect()
    local carte = AskForTarget(1)                                                   --carte
    EffectOwner.OtherPlayer.Hand.MoveTo(EffectOwner.OtherPlayer.Discard,carte,0)    --prends la carte de l'adversaire depuis la main et la met dans sa defausse
end