# ClickTrack

Really simple click tracker:

- No need to register urls, any url will be tracked.
- Only number of clicks per URL collected, no extra data like unique users or addresses.

## Usage

Just append any URL to the track endpoint like this:

```text
http://localhost:5000/track/https://google.com
```

Then you can retrieve all the clicks using this other endpoint:

```text
http://localhost:5000/clicks
```

Also with pagination in case you have been using the service for a while and the results are too many to send in a single operation:

```text
http://localhost:5000/clicks?limit=20&page=3
```

First page is page 1.

You can also retrieve the data for a specific URL:

```text
http://localhost:5000/clicks/https://google.com
```

## UI

Coming soon, for now just check the results in bulk.
