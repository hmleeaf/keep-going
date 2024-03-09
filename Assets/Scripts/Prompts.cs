using System.Collections.Generic;
using UnityEngine;
using static Prompts;

public static class Prompts
{
    public enum Tone
    {
        Encouraging, 
        Indifferent, 
        Condescending, 
        Manipulative, 
        Doubtful
    }

    static Dictionary<Tone, string[]> tonePrompts = new Dictionary<Tone, string[]>()
    {
        { 
            Tone.Encouraging, 
            new string[] 
            {
                "You can do it.",
                "Good job. Keep going.",
                "You’ve done so well.",
                "This is fun, eh?",
                "Do your thing.",
                "Nice work. Keep going.",
                "Impressive.",
                "You are doing so well.",
            }
        },
        { 
            Tone.Indifferent, 
            new string[]
            {
                "You’ve come this far? Okay.",
                "Go ahead and pick up the pace.",
                "Don’t fall behind now.",
                "Wow nice. Now keep going.",
                "You’re doing alright. Keep at it.",
                "You will keep going.",
            }
        },         { 
            Tone.Condescending, 
            new string[]
            {
                "What’s the matter now? Keep going.",
                "You’re falling behind. Go faster.",
                "You used to be better.",
                "Do better like everyone else.",
                "Wow. I thought you’re better than this.",
                "For how long you’ve been doing this, I expected more from you.",
                "This is all you can do? Keep going.",
                "You want a treat or something? Keep going.",
                "How are you this bad? Look at everyone around you.",
            }
        },         { 
            Tone.Manipulative, 
            new string[]
            {
                "This is all you will do now.",
                "You can’t do anything else.",
                "Don’t even think about turning back.",
                "If you give up now, you’ll be a failure.",
                "You will keep going, just like everyone else.",
                "It’s too late to turn back.",
                "You won’t succeed at anything else.",
                "This is what everyone does. Who do you think you are to be different.",
                "Do you think you’re better than everyone else?",
                "Going back means failing.",
                "You’re better than giving up.",
                "Your life will be destroyed if you give up now.",
                "You don't have the courage to be different.", 
            } 
        },         { 
            Tone.Doubtful, 
            new string[]
            {
                "What are you doing?",
                "I can’t do this.",
                "What will you do now?",
                "I’m not made for this.",
                "Am I made for this?",
                "I’m not cut out for this.",
                "You’re not cut out for this.",
                "What do you think you’re doing?",
                "Don’t be special now.",
                "Do you think you’re special?", 
            } 
        }, 
    };

    public enum Condition
    {
        WrongDirection,
        InFog,
        InDeepFog,
        OmitTutorialCrateCoin,
        ProgressCanTransition,
    }

    static Dictionary<Condition, string[]> conditionPrompts = new Dictionary<Condition, string[]>()
    {
        {
            Condition.WrongDirection,
            new string[]
            {
                "Hey, wrong way.",
                "You are supposed to go forward.", 
                "Forward, buddy."
            }
        },
        {
            Condition.InFog,
            new string[] 
            {
                "Don't go there.", 
                "Come back out,", 
                "Doesn't feel right in there, does it?", 
            }
        }, 
        { 
            Condition.InDeepFog, 
            new string[]
            {
                "You need to get out now.",
                "Don't do this to yourself.", 
                "Why make yourself suffer?", 
            }
        },        
        { 
            Condition.OmitTutorialCrateCoin, 
            new string[]
            {
                "Or don't. Fine.", 
                "Or don't. You're your own person I guess.", 
                "Or don't. I can't control you.", 
            }
        },        
        { 
            Condition.ProgressCanTransition, 
            new string[]
            {
                "Whatever you do, do not turn back.", 
                "Horrible things will happen to you if you go back now.", 
                "Don't be different now. Don't back out.", 
                "There is no going back now.", 
                "You don't know what terrible things await you there.", 
            }
        },
    };

    public static string GetPrompt(Tone tone)
    {
        string[] prompts = tonePrompts[tone];
        return prompts[Random.Range(0, prompts.Length)];
    }

    public static string GetPrompt(Condition condition)
    {
        string[] prompts = conditionPrompts[condition];
        return prompts[Random.Range(0, prompts.Length)];
    }
}