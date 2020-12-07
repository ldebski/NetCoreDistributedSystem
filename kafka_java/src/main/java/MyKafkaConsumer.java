//import org.apache.kafka.clients.consumer.ConsumerConfig;
//// import org.apache.kafka.clients.consumer.ConsumerRecord;
//// import org.apache.kafka.clients.consumer.ConsumerRecords;
//// import org.apache.kafka.clients.consumer.KafkaConsumer;
//// import org.apache.kafka.common.serialization.StringDeserializer;
//// import org.slf4j.Logger;
//// import org.slf4j.LoggerFactory;
////
//// import java.lang.reflect.Array;
//// import java.time.Duration;
//// import java.util.Arrays;
//// import java.util.Collections;
//// import java.util.Properties;
////
//// public class MyKafkaConsumer {
////     Properties props;
////     KafkaConsumer<String, String> consumer;
////     Logger logger;
////
////     public MyKafkaConsumer(){
////         this.logger = LoggerFactory.getLogger(MyKafkaConsumer.class.getName());
////
////         //consumer config
////         this.props = new Properties();
////         props.setProperty(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, "localhost:9092");
////         props.setProperty(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
////         props.setProperty(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
////         props.setProperty(ConsumerConfig.GROUP_ID_CONFIG, "consumer-stream-group");
////         props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "latest");
////
////         consumer = new KafkaConsumer<String, String>(props);
////         consumer.subscribe(Collections.singletonList("my_topic"));
////     }
////
////     // public String poolMessage(){
////     //     ConsumerRecords<String, String> record = consumer.poll(Duration.ZERO);
////     //
////     // }
//
// }
