EXTERNAL battleStart()
EXTERNAL npcAction(name)
EXTERNAL questItem(itemName)
EXTERNAL getQuestStatus(name)
EXTERNAL setQuestStatus(name, name)
EXTERNAL debug(value)


VAR trial1choice = ""
VAR trial2choice = ""
VAR trial3choice = ""
VAR option = 0
VAR questStatus = ""
VAR pass = 0
~ getQuestStatus("questWudang")
-> main

=== main ====
~ getQuestStatus("questWudang")
{questStatus == "success":
    I will see you at the Alliance, Jin.
    ->END
}
{questStatus == "fail":
   You are not yet ready to carry the weight of Wudang’s support.
   Return when you have tempered both your spirit and your sword.
   ->END
}
{questStatus == "unobtained":
What brings you to Mount Wudang young one?
*[Greetings to the "Righteous Sword"]
Haha I rarely hear that name nowadays
**[The Alliance needs your help]
***[The Demonic sect has made a return]
!
The Demonic Sect...
Apologies but our Wudang sect is weak right now, I don't think we can give you the assistance you hope for.
****[We very much need it]
Hmmm....
If I may be honest, I don't trust the Alliance.
I fear that it's become corrupt from too much money and the power...
Young man. What position do you hold in the Alliance?
*****[I am the disciple of the Alliance leader]
I see...
Then you are qualified enough for me to test.
Pass a series of trials and I shall trust the Alliance once again.
Fail and I shall consider the Alliance corrupt!
******[I understand]
-> trial1
}

=== trial1 ===
[Pass 2 of the 3 trials to convince Zhang Tianyang "Righteous Sword" the Alliance is still righteous!]
Well then.. The first trial.
The first trial is Reflection. To see the world clearly, you must first see yourself. 
Each reflection shows a different facet of your soul. Which version of you will lead the others?
*[Choose the Angry reflection]
Anger may give strength in battle, but it clouds the mind. Are you driven by fury or by a sense of justice? This is not the Wudang way. Let us hope your next trials will show clearer vision.
    ~ trial1choice = "fail"
    -> trial2

*[Choose the Fearful reflection]
~ setQuestStatus("questWudang","fail")
Fear keeps us alive but also chains us. Can you act in spite of your fears, or will they paralyze you when the time comes to defend Murim? Let us hope your next trials will show clearer vision.
    ~ trial1choice = "fail"
    -> trial2

*[Choose the Calm reflection]
You have chosen wisely. Calmness in the face of adversity is the mark of a true martial artist. Remember, in the chaos of battle, stillness is your greatest weapon.
    ~ trial1choice = "pass"
    ~ pass +=1
    -> trial2
    
=== trial2 ===
In life, we often face situations where the path ahead is not always clear. In these moments, wisdom is required, not brute strength.
You must show me that you can balance thought and action, that you understand the harmony of yin and yang.
For this trial, you will be faced with choices where success depends not only on your answer but on your understanding of balance.
You have trained hard, but your body is fatigued.
You have one final chance to push yourself to break through your limits, but doing so risks long-term injury. 
If you rest, you may regain your strength, but you will lose the chance to advance immediately.
What do you do?

*[Meditate to find inner strength]
    **[To avoid both injury and delay.]
    Your choice reflects Wudang’s core teachings of balance and seeking strength within.
    ~trial2choice = "pass"
    ~ pass +=1
    -> trial3
    
*[Push through the pain and risk injury]
    **[The potential rewards are great.]
    Your choice demonstrates ambition but risks imbalance by pushing too hard.
    ~trial2choice = "fail"
    -> trial3

*[Rest and recover]
    **[Rushing may cause long-term damage.]
    Your choice shows patience, which aligns with the philosophy of long-term growth.
    ~trial2choice = "fail"
    -> trial3


->END

=== trial3 ===
The final trial is The Sword. Show me that you have learned not only strength but restraint. Prove to me that your blade serves justice and not ambition.
We will engage in a duel choose your actions wisely.
*[Attempt to strike first]
    [Master Tianyang blocks your strike but is left off-balance]
    **[Strike again but harder]
        You wield your sword as if it were a hammer, reckless and unchecked. 
        Power without wisdom is a danger to yourself and others. 
        To carry a blade is to shoulder responsibility, not to thirst for victory. 
        You must learn that the sharpest edge can cut both ways. You have failed this trial.
        ~trial3choice = "fail"
        -> trialconclusion
    **[Prepare to block]
        Caution is not the same as wisdom. 
        Inaction in the face of injustice is the same as complicity. 
        A sword that remains idle while the innocent suffer is no better than one drawn in anger. 
        Balance is not passivity, and restraint is not hesitation. You have failed this trial.
        ~trial3choice = "fail"
        -> trialconclusion
    **[Feint a block and strike swiftly]
        You understand. The sword is neither a tool of war nor a token of peace. 
        It is an extension of your spirit, and through balance, you control it. 
        Restraint when necessary, action when required—this is the true path of justice. 
        You have proven yourself worthy of Wudang’s teachings.
        ~trial2choice = "pass"
        ~ pass +=1
        -> trialconclusion

*[Await the master's first move]
    [Master Tianyang strikes you fast but not hard]
    **[Strike back at him with full force]
        You wield your sword as if it were a hammer, reckless and unchecked. 
        Power without wisdom is a danger to yourself and others. 
        To carry a blade is to shoulder responsibility, not to thirst for victory. 
        You must learn that the sharpest edge can cut both ways. You have failed this trial.
        ~trial3choice = "fail"
        -> trialconclusion
    **[Jump back to gain distance]
        Caution is not the same as wisdom. 
        Inaction in the face of injustice is the same as complicity. 
        A sword that remains idle while the innocent suffer is no better than one drawn in anger. 
        Balance is not passivity, and restraint is not hesitation. You have failed this trial.
        ~trial3choice = "fail"
        -> trialconclusion
    **[Strike with intention to parry his next move]
        You understand. The sword is neither a tool of war nor a token of peace. 
        It is an extension of your spirit, and through balance, you control it. 
        Restraint when necessary, action when required—this is the true path of justice. 
        You have proven yourself worthy of Wudang’s teachings.
        ~trial2choice = "pass"
        ~ pass +=1
        -> trialconclusion

=== trialconclusion === 
~ debug(pass)
{pass <=1:
You have come far, Jin.
Though you possess strength and skill, your heart and mind are not yet aligned with the teachings of Wudang. 
The blade you wield still moves with doubt, aggression, or hesitation, traits that will betray you in moments of true danger.
Only when you walk the path of balance, will Wudang stand by your side in the coming storm.
~ setQuestStatus("questWudang", "fail")
}
{pass == 2:
    Jin, you have shown great promise.
    Your strength and resolve are undeniable, and in two of the trials, you have demonstrated the wisdom and restraint necessary to wield the sword of justice.
    Yet, there remains a shadow of doubt—an edge that still cuts too quickly or a hand that sometimes falters.
    For now, Wudang will lend its strength to your cause.
    But remember, the journey does not end here. 
    Continue to sharpen your spirit as you would your sword. Only then will you be truly worthy of the path you walk.
    ~ setQuestStatus("questWudang", "success")
}
{pass == 3:
    You have done well, Jin. In each trial, you have shown not only the strength of your body but the wisdom of your heart. 
    You fought not for glory, but for justice. You wield your blade not as a tool of destruction, but as a guardian of life.
    You have mastered the balance between action and restraint, between power and peace.
    Wudang stands with you, Namgung Jin, and I will join you at the Murim Alliance.
    Let your heart guide your sword, and remember, true strength lies not in victory, but in the peace you leave behind.
    ~ setQuestStatus("questWudang", "success")
}
->END