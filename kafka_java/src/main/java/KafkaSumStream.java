import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.streams.KafkaStreams;
import org.apache.kafka.streams.StreamsBuilder;
import org.apache.kafka.streams.StreamsConfig;
import org.apache.kafka.streams.kstream.KStream;

import java.util.Properties;

public class KafkaSumStream {
    public static void main(String[] args) {
        // create properties
        Properties properties = new Properties();
        properties.setProperty(StreamsConfig.BOOTSTRAP_SERVERS_CONFIG, "localhost:9092");
        properties.setProperty(StreamsConfig.APPLICATION_ID_CONFIG, "kafka-group6");
        properties.setProperty(StreamsConfig.DEFAULT_KEY_SERDE_CLASS_CONFIG, Serdes.StringSerde.class.getName());
        properties.setProperty(StreamsConfig.DEFAULT_VALUE_SERDE_CLASS_CONFIG, Serdes.StringSerde.class.getName());
        properties.put(StreamsConfig.CACHE_MAX_BYTES_BUFFERING_CONFIG, 0);

        StreamsBuilder streamsBuilder = new StreamsBuilder();

        KStream<Object, Object> extractedStream = streamsBuilder.stream("firstTopic")
                .groupByKey()
                .reduce((v1, v2) ->  // agg_value, newValue
                        MyJsonParser.extractValueFromMessages(v1.toString(), v2.toString()))
                .filter((k, v) -> !MyJsonParser.isRet(v.toString()))
                .toStream()
                .filter((k, v) -> v != null);

        // send back messages to increase transaction receiver account balance
        extractedStream
                .filter((k, v) -> MyJsonParser.isDone(v.toString()))
                .selectKey((k, v) -> MyJsonParser.getToID(v.toString()))
                .mapValues(v -> MyJsonParser.swapValue(v.toString()))
                .to("firstTopic");

        // send back return message to API
        extractedStream
                .to("secondTopic");


        KafkaStreams kafkaStreams = new KafkaStreams(streamsBuilder.build(), properties);

        kafkaStreams.start();
    }
}
