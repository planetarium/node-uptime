namespace NodeUptime.Client;

public static class GraphQLQueries
{
    public const string GetTip =
        @"
            query GetTip {
                nodeStatus {
                    tip {
                        index
                    }
                }
            }";

    public const string GetBlockTimestamp =
        @"
            query {
                chainQuery {
                    blockQuery {
                        blocks(desc: true, offset: 0, limit: 1) {
                            timestamp
                        }
                    }
                }
            }
        ";
}
