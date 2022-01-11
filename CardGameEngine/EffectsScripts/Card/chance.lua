max_level = 2
image_id = 709

name = "chance"
pa_cost = 2

description = "Devine une carte de la main du joueurs si c'est bon elle va dans sa defausse"

function cardFilter()
    local OtherHand = EffectOwner.OtherPlayer.Hand
    if (OtherHand.Count == 0) then
        return nil
    else
        --TODO 
    end
end

targets = {
    CreateTarget("Devine une carte de la main du joueurs", TargetTypes.Card, true, cardFilter),
}


function precondition()
    return EffectOwner.OtherPlayer.Hand.Count > 0
end

function do_effect()
    --TODO
    local carte = AskForTarget(1)                                               --carte
    EffectOwner.OtherPlayer.Hand.MoveTo(EffectOwner.OtherPlayer.Discard, carte, 0)     --prends la carte de l'adversaire depuis la main et la met dans sa defausse
end