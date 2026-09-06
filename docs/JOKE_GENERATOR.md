# Joke Generator Service - RasmiSupermarket

## Overview

A fun microservice that integrates with external joke APIs to provide random jokes to users. This service demonstrates how to integrate third-party APIs into the RasmiSupermarket system.

## Features

- Random joke generation from multiple sources
- Caching to reduce API calls
- Multiple joke types support
- Error handling and fallback mechanisms

## Available Endpoints

### Get Random Joke

```
GET /api/jokes/random
```

#### Response
```json
{
  "success": true,
  "message": "Joke retrieved successfully",
  "data": {
    "id": 1,
    "setup": "Why did the programmer quit his job?",
    "punchline": "Because he didn't get arrays.",
    "type": "programming",
    "source": "JokeAPI"
  }
}
```

### Get Joke by Category

```
GET /api/jokes/category/{category}
```

#### Parameters
- `category`: Type of joke (programming, knock-knock, dad, etc.)

#### Response
```json
{
  "success": true,
  "message": "Joke retrieved successfully",
  "data": {
    "id": 2,
    "setup": "Knock knock",
    "punchline": "Who's there? Interrupting programmer. Interrupting programmer w—. SYNTAX ERROR!",
    "type": "knock-knock",
    "source": "JokeAPI"
  }
}
```

## API Sources

### JokeAPI
- **URL**: https://jokeapi.dev/api/joke/Any
- **Types**: Programming, Knock-Knock, General, Dad Jokes
- **Format**: JSON with setup and delivery

### Benefits
- No authentication required
- Multiple category support
- High availability
- JSON format

## Configuration

### appsettings.json
```json
{
  "ExternalApis": {
    "JokeApi": {
      "BaseUrl": "https://jokeapi.dev/api/joke",
      "Timeout": 5000,
      "CacheDurationMinutes": 60
    }
  }
}
```

## Usage Examples

### C# Example
```csharp
var jokeService = serviceProvider.GetRequiredService<IJokeService>();

// Get random joke
var response = await jokeService.GetRandomJokeAsync();
if (response.Success)
{
    Console.WriteLine(response.Data.Setup);
    Console.WriteLine(response.Data.Punchline);
}

// Get joke by category
var categoryJoke = await jokeService.GetJokeByCategoryAsync("programming");
```

### cURL Example
```bash
# Get random joke
curl -X GET "https://localhost:5001/api/jokes/random" \
  -H "Authorization: Bearer {token}"

# Get programming joke
curl -X GET "https://localhost:5001/api/jokes/category/programming" \
  -H "Authorization: Bearer {token}"
```

## Error Handling

### Fallback Jokes
If the external API is unavailable, the service provides fallback jokes:

```csharp
var fallbackJoke = new JokeDto
{
    Setup = "Why did the joke generator fail?",
    Punchline = "Because the API went down! 😄",
    Type = "fallback",
    Source = "Local"
};
```

### Error Response
```json
{
  "success": false,
  "message": "Unable to fetch joke from external API",
  "data": null,
  "errors": ["JokeAPI request failed"],
  "timestamp": "2026-09-06T22:46:24Z"
}
```

## Performance Considerations

### Caching Strategy
- Cache jokes for 1 hour
- Reduce unnecessary API calls
- Improve response time
- Reduce external API load

### Cache Implementation
```csharp
private readonly IMemoryCache _cache;

private async Task<JokeDto> GetOrCacheJoke(string cacheKey)
{
    if (_cache.TryGetValue(cacheKey, out JokeDto? cachedJoke))
    {
        return cachedJoke!;
    }
    
    var joke = await FetchFromExternalApi();
    _cache.Set(cacheKey, joke, TimeSpan.FromHours(1));
    return joke;
}
```

## Integration Points

### Dashboard Integration
Display daily funny fact on the admin dashboard

### Email Notifications
Include a "Joke of the Day" in daily reports

### Mobile App
Show random jokes between transactions

### Team Engagement
Share jokes in internal communications

## Supported Joke Categories

1. **Programming**: Tech and coding jokes
2. **Knock-Knock**: Classic knock-knock format
3. **Dad Jokes**: Family-friendly humor
4. **General**: Mixed category jokes
5. **Pun**: Wordplay and puns
6. **Dark**: Dark humor

## Rate Limiting

### JokeAPI Limits
- No rate limiting for free tier
- Unlimited requests
- Public API with no authentication

### Application Level
- Implement request throttling per user
- Maximum 10 jokes per minute per user
- Daily limit: 1000 jokes per user

## Future Enhancements

1. **Multi-Source Support**
   - Add Chuck Norris jokes API
   - Add Quotes API
   - Add Riddles API

2. **Advanced Filtering**
   - Filter by joke length
   - Filter by rating/safety level
   - Filter by language

3. **User Preferences**
   - Save favorite jokes
   - Track joke history
   - Personalized recommendations

4. **Analytics**
   - Track most popular jokes
   - User engagement metrics
   - API response time monitoring

5. **Contribution System**
   - Allow users to submit jokes
   - Community-curated content
   - Moderation workflow

## Troubleshooting

### Issue: Jokes not loading
**Solution**: 
1. Check internet connection
2. Verify external API is accessible
3. Check application logs for errors
4. Ensure cache is properly configured

### Issue: Slow response time
**Solution**:
1. Enable caching
2. Check network latency
3. Verify API endpoint is responsive
4. Consider implementing CDN

### Issue: Always getting same joke
**Solution**:
1. Clear application cache
2. Verify cache timeout settings
3. Implement cache invalidation logic
4. Check for caching headers from external API

## Security Considerations

1. **Input Validation**: Validate category parameters
2. **Output Sanitization**: Clean joke content before display
3. **HTTPS Only**: Use secure connections to external APIs
4. **API Key Protection**: If authentication is needed
5. **CORS Configuration**: Restrict cross-origin requests

---

**Version**: 1.0.0  
**Last Updated**: 2026-09-06
