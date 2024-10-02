EXTERNAL npcAction(name)
EXTERNAL battleStart()
EXTERNAL getQuestStatus(questName)
EXTERNAL setQuestStatus(questName, questStatus)
EXTERNAL giveQuestItem(itemName)
VAR option = 0
VAR questStatus = ""

-> fightover
=== fightover === 
{questStatus == "complete":
[The bandit drops to his knees, clutching his wounds.]
Alright, alright... You win, kid. Just... let me go, okay? Here, take this scroll. It’s not worth dying for.
[The bandit hands over a Martial Art Scroll reluctantly.]
    ~ giveQuestItem("MartialArtScroll")
    ~ setQuestStatus("questBandit", "completeW")
    -> END
}
{questStatus == "lost":
[The bandit stands over you, triumphant.]
With this Martial Art scroll, I’m unstoppable! Now get lost before I finish the job!
    ~ setQuestStatus("questBandit", "completeF")
    -> END
}

-> main
=== main ===
~ getQuestStatus("questBandit")
{questStatus == "completeW":
I’ve got nothing left for you! Now get lost!
    -> END
}
{questStatus == "completeF":
Still hanging around, kid? Want another taste of my fists? Get out of here!
    -> END
}
~ getQuestStatus("questMountHua")
{questStatus == "unobtained":
What do you want kid?
*[What are you doing in this area?]
[The bandit laughs, his eyes narrowing as he looks you up and down.]
None of your business, kid. Now scram before I tell my boys to rough you up!
    **[Attack the bandit]
        ~ option = 1
        -> chosen
    **[Leave]
    -> END
}
{questStatus == "incomplete":
What do you want kid?
[The bandit eyes you warily, but there's a sneer in his voice.]
*[Do you have a Martial art scroll!]
[The bandit’s eyes widen for a split second before he regains his composure, crossing his arms.]
And what if I do? What’s it to you?
**[Give me the scroll!]
[The bandit bursts into laughter, clearly not taking you seriously.]
HA! And why should I just hand it over to a little runt like you?
    ***[Attack the bandit]
        ~ option = 1
        -> chosen
    ***[Leave]
        [The bandit watches you go, shaking his head in amusement.]
        That’s right! Run along before things get ugly!
    -> END
}

=== chosen ===
{option == 1:
[You step forward, challenging the bandit directly.]
[The bandit grins, cracking his knuckles.]
Oh, you’re in for a world of hurt!
    ~ battleStart()
}
-> END