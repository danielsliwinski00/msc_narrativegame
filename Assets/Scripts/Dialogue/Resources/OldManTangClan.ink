VAR choice = 0
EXTERNAL giveQuestItem(itemName)

-> main

=== main ===
Hello there, traveler.
Could you spare a moment to listen to an old man’s ramblings?
    + [Yes, I can listen.]
    ~ choice = 0
        -> chosen
    + [No, I don’t have time for this.]
    ~ choice = 1
        -> chosen

=== chosen ===
{choice == 0:
[The old man’s face softens with relief.]
Thank you, most travelers hurry past without a second thought. It’s nice to have someone listen for a change. 
Did you, by any chance, come here to meet with the Tang Clan?
    + [Yes, I came to see the patriarch]
    ~ choice = 2
    -> continuation
    + [No, I’m not here to see the Tang Clan.]
    ~ choice = 3
    -> continuation
}
{choice == 1:
[The old man’s shoulders slump slightly.]
Oh... I see. Well, forgive me for wasting your time.
-> END
}
=== continuation ===
{choice == 2:
[The old man nods, his eyes growing serious.]
Ah, I thought so. You’ll need to be cautious around them. The Tang Clan specializes in poison and hidden weapons—deadly arts passed down for generations. 
The young masters can be... prideful and quick to temper. But they are strong, so it’s best not to provoke them.
[He pauses, glancing toward the distant mountains.]
Lately, I’ve heard whispers of unusual creatures roaming the lands nearby. Be vigilant, traveler. And for listening to an old man’s worries, here—take this.
[He hands you a small snack wrapped carefully in cloth.]
May it give you strength on your journey.
    ~ giveQuestItem("Snack")
    -> END
}
{choice == 3:
[The old man raises an eyebrow but nods understandingly.]
I see... Well, regardless of your reasons, be careful if you do cross paths with them. The young masters have little patience for outsiders.
[He gives you a wary look before continuing.]
Their arrogance can be dangerous, but so too can their skill with poison and hidden blades. Tread lightly, traveler.
}
-> END
