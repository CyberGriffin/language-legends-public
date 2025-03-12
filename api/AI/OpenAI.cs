using Azure.AI.OpenAI;
using API.Models;

namespace API.AI;

public class OpenAI {
    public string systemPrompt = "";
    private readonly OpenAIClient client;
    private readonly ChatCompletionsOptions options;

    public OpenAI(string systemPrompt, Conversation[] conversationHistory = null) {
        this.systemPrompt = systemPrompt;
        client = new OpenAIClient(
            Environment.GetEnvironmentVariable("RPGMAKER_OPENAI_API_KEY") ??
            throw new Exception("RPGMAKER_OPENAI_API_KEY environment variable not set.")
        );
        options = new ChatCompletionsOptions {
            Temperature = 0.9f,
            MaxTokens = 600,
            FrequencyPenalty = 0.0f,
            PresencePenalty = 0.6f,
            DeploymentName = "gpt-4o-mini"
        };

        options.Messages.Add(new ChatRequestSystemMessage(systemPrompt));

        if (conversationHistory != null) {
            foreach (var conversation in conversationHistory) {
                options.Messages.Add(new ChatRequestUserMessage(conversation.UserMessage));
                options.Messages.Add(new ChatRequestSystemMessage(conversation.ResponseMessage));
            }
        }
    }

    public async Task<string> Query(string query) {
        if (string.IsNullOrEmpty(query.Trim())) return "";
        options.Messages.Add(new ChatRequestUserMessage(query));

        int currentRetry = 0;
        while (currentRetry <= 3) {
            try {
                using var cts = new CancellationTokenSource();
                var responseTask = client.GetChatCompletionsStreamingAsync(options, cts.Token);
                var completedTask = await Task.WhenAny(responseTask, Task.Delay(2000, cts.Token));
                if (completedTask == responseTask) {
                    var response = responseTask.Result;
                    string fullResponse = "";
                    await foreach (var update in response) {
                        if (update.ContentUpdate != null) {
                            fullResponse += update.ContentUpdate.ToString();
                        }
                    }

                    options.Messages.Add(new ChatRequestUserMessage(fullResponse));
                    return fullResponse;
                } else {
                    cts.Cancel();
                    throw new TaskCanceledException();
                }
            } catch (TaskCanceledException tce) when (tce.CancellationToken.IsCancellationRequested) {
                ++currentRetry;
                await Backoff(currentRetry);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        return null;
    }

    private static async Task Backoff(int currentRetry) {
        int backoff = (int)((Math.Pow(2, currentRetry) - 1) * 1000);
        await Task.Delay(backoff);
    }
}
