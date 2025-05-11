import {Client, Events, GatewayIntentBits } from "discord.js";
import config from "../config.json";

const client = new Client({intents: GatewayIntentBits.Guilds});

client.once(Events.ClientReady, readyClient => {
    console.log("Ready!");
});

void client.login(config.token);