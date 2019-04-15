# KafkaSignalR
## 基礎設施架設
* Zookeeper    
```
docker run -d --name zk -d --restart always -p 2181:2181 \
 -e ZOO_MY_ID="1" \
 -e ZOO_SERVERS="server.1=127.0.0.1:2888:3888" \
 -e ZOO_AUTOPURGE_SNAP_RETAIN_COUNT="3" \
 -e ZOO_AUTOPURGE_PURGE_INTERVAL="1" \
 -e ZOO_MAX_CLIENT_CNXNS="0" \
 zookeeper:3.4.10
```
* Kafka
```
docker run -d --name kafka -d -p 9092:9092 -p 15000:15000 --link zk:zk \
 -e KAFKA_ADVERTISED_HOST_NAME="127.0.0.1" \
 -e KAFKA_ADVERTISED_PORT="9092" \
 -e KAFKA_BROKER_ID="1" \
 -e KAFKA_DEFAULT_REPLICATION_FACTOR="1" \
 -e KAFKA_DELETE_TOPIC_ENABLE="true" \
 -e KAFKA_LOG_RETENTION_HOURS="24" \
 -e KAFKA_LOG_ROLL_HOURS="1" \
 -e KAFKA_LOG_SEGMENT_DELETE_DELAY_MS="1" \
 -e KAFKA_NUM_PARTITIONS="18" \
 -e KAFKA_ZOOKEEPER_CONNECT="zk:2181" \
 -e KAFKA_HEAP_OPTS="-Xmx1G -Xms1G" \
 -e JMX_PORT="9988" \
 wurstmeister/kafka:2.12-2.1.1
```
* 本地端編譯執行KafkaSignalR.sln
* Vue SignalR Demo
```
cd kafkasignalrvue
npm install
npm run dev
```
* 藉由觸發WebApi -> PubMessage to Kafka -> KafkaEventHandler -> SignalRBocast完成測試週期
```
PostUrl: host/api/PubMessage
RequestData: {
    Message: string
}
```