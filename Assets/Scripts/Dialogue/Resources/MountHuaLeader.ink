EXTERNAL battleStart()
EXTERNAL npcAction(name)
EXTERNAL questItem(itemName)
EXTERNAL takeQuestItem(itemName)
EXTERNAL setQuestStatus(name, name)
EXTERNAL getQuestStatus(name)
~ questItem("MartialArtScroll")
VAR option = 0
VAR questItem = 1
VAR questStatus = ""

-> main

=== main ===
~ getQuestStatus("questMountHua")
{questStatus == "unobtained":
Ah, it's a pleasant day today, isn't it?
[He glances at you with a warm smile.]
    *[I greet the Mount Hua sect master]
    Ah, what brings you here, traveler? You look like you’ve got something on your mind.
    **[I'm here on behalf of the Alliance Leader]
Hoho, the Alliance Leader, you say? Now that’s interesting. What news does he send?
    ***[The Demonic sect has made a return]
    ****[And we require your help!]
    [The Sect Master’s expression shifts, becoming more serious.]
The Demonic Sect, you say? Hmm, this is no small matter. I would be honored to lend my strength...
However, I find myself tied down here at the sect.
You see, one of my junior disciples misplaced a precious martial arts scroll of ours. We can’t afford to lose it, and I can’t leave until it’s recovered.
    ~ getQuestStatus("questBandit")
    {questStatus == "completeW":
        *[It wouldn't happen to be this one?]
        [His eyes widen in surprise.]
        My goodness, you found it! Where on earth did you come across this?
        **[A group of bandits got a hold of it]
        Bandits, huh? Well, I suppose that doesn’t surprise me. What matters is that it's back in our hands. Now, I can leave for the Alliance without any concerns!
        [He smiles, clearly relieved.]
        Thank you, Jin. I'll head to the headquarters immediately.
        ~ setQuestStatus("questMountHua", "success")
        ->END
    }
    ~ getQuestStatus("questBandit")
    {questStatus == "completeF":
        *[I think I saw some bandits with it]
        Ah, good, so you know where it is. But...
        [His expression darkens.]
        Why don't you have it?
        **[They beat me in a duel]
        [He sighs heavily and shakes his head.]
        I see. Unfortunately, that leaves me no choice but to handle those bandits myself. I won’t have time to visit the Alliance until this matter is dealt with.
        [He looks disappointed.]
        I’m afraid you’ve failed this task.
        ~ setQuestStatus("questMountHua", "fail")
        ->END
    }
    If you can manage to track down that scroll for me, I will leave for the Alliance right away!
    [He pauses, his eyes narrowing slightly.]
    Honestly speaking... I believe some bandits may have been involved. If you find it in their hands, make sure to retrieve it!
    ~ setQuestStatus("questMountHua", "incomplete")
    ->END
}
{questStatus == "incomplete":
~ getQuestStatus("questBandit")
Have you had any luck?
    {questStatus == "completeW":
    *[It wouldn't happen to be this one?]
        [His eyes widen in surprise.]
    My goodness, you found it! Where on earth did you come across this?
        **[A group of bandits got a hold of it]
        Bandits, huh? Well, I suppose that doesn’t surprise me. What matters is that it's back in our hands. Now, I can leave for the Alliance without any concerns!
        [He smiles, clearly relieved.]
        Thank you, Jin. I'll head to the headquarters immediately.
        ~ setQuestStatus("questMountHua", "success")
        ->END
    }
    {questStatus == "completeF":
        *[I saw some bandits with it]
        Ah, good, so you know where it is. But...
        [His expression darkens.]
        Why don't you have it?
        **[They beat me in a duel]
        [He sighs heavily and shakes his head.]
I see. Unfortunately, that leaves me no choice but to handle those bandits myself. I won’t have time to visit the Alliance until this matter is dealt with.
[He looks disappointed.]
I’m afraid you’ve failed this task.
        ~ setQuestStatus("questMountHua", "fail")
        ->END
    }
}
{questStatus == "success":
I'll be leaving shortly, Jin. The Alliance will see me there soon!
-> END
}
{questStatus == "fail":
I’m sorry, Jin, but I’m afraid I’m tied up with matters here. The Alliance will have to wait.
-> END
}